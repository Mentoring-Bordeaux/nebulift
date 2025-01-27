<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import type { TemplateSchema } from '@/types/template';
import { useTemplateConfigStore } from '@/stores/templateConfigStore';
import TechnologySection from './TechnologySection.vue';
import Review from '../ReviewConfiguration.vue';

const store = useTemplateConfigStore();

// Define error state type
type ErrorState = Record<string, Record<string, string>>;
const errors = ref<ErrorState>({});
const showReview = ref(false);
const activeTab = ref<string>(Object.keys(store.schema || {})[0] || '');

const links = computed(() => {
  if (!store.schema) return [];
  return Object.keys(store.schema).map((sectionKey) => ({
    label: store.schema![sectionKey].title,
    value: sectionKey,
  }));
});

const sections = computed(() => {
  if (!store.schema) return [];
  return Object.keys(store.schema);
});
const currentSectionIndex = computed(() =>
  sections.value.indexOf(activeTab.value)
);
const isFirstSection = computed(() => currentSectionIndex.value === 0);
const isLastSection = computed(
  () => currentSectionIndex.value === sections.value.length - 1
);
const isSectionValid = computed(() => {
  const sectionKey = activeTab.value;
  const section = store.schema?.[sectionKey];

  if (!section) return false;

  return Object.keys(section.properties).every((fieldKey) => {
    const value = store.formData[sectionKey]?.[fieldKey];
    const config = section.properties[fieldKey];

    if (Array.isArray(value)) {
      const minItems = config.minItems === undefined ? 1 : config.minItems;
      const filledItemsCount = value.filter(
        (item) => item && item.trim() !== ''
      ).length;
      return filledItemsCount >= minItems;
    }

    return value && value.trim() !== '';
  });
});

const goToNextSection = () => {
  if (!isLastSection.value) {
    activeTab.value = sections.value[currentSectionIndex.value + 1];
  }
};

const goToPreviousSection = () => {
  if (!isFirstSection.value) {
    activeTab.value = sections.value[currentSectionIndex.value - 1];
  }
};

// Function to validate active section
const validateSection = (): boolean => {
  const sectionKey = activeTab.value;
  const section = store.schema?.[sectionKey];

  if (!section) return false;

  errors.value[sectionKey] = {};

  let hasErrors = false;

  Object.entries(section.properties).forEach(([fieldKey, config]) => {
    const value = store.formData[sectionKey]?.[fieldKey];

    if (Array.isArray(value)) {
      const minItems = config.minItems === undefined ? 1 : config.minItems;
      const filledItemsCount = value.filter(
        (item) => item && item.trim() !== ''
      ).length;

      if (filledItemsCount < minItems) {
        errors.value[sectionKey][fieldKey] =
          `At least ${minItems} item(s) must be filled`;
        hasErrors = true;
      }
    } else if (!value || value.trim() === '') {
      errors.value[sectionKey][fieldKey] = 'This field is required';
      hasErrors = true;
    }
  });

  Object.keys(store.formData).forEach((sectionKey) => {
    Object.keys(store.formData[sectionKey]).forEach((fieldName) => {
      if (Array.isArray(store.formData[sectionKey][fieldName])) {
        store.formData[sectionKey][fieldName] = store.formData[sectionKey][
          fieldName
        ].filter((item: string) => item !== '');
      }
    });
  });

  return !hasErrors;
};

const handleNext = () => {
  if (validateSection()) {
    if (isLastSection.value) {
      showReview.value = true;
    } else {
      goToNextSection();
    }
  }
};

const handleBack = () => {
  goToPreviousSection();
};

// Type-safe form value update handler
const handleFormUpdate = (
  sectionKey: string,
  value: Record<string, string | string[]>
): void => {
  store.formData[sectionKey] = value;
};

// Watch for schema changes
watch(
  () => store.schema,
  (newSchema) => {
    if (newSchema) {
      errors.value = {};
      activeTab.value = Object.keys(newSchema)[0] || '';
    }
  },
  { immediate: true }
);
</script>

<template>
  <div class="max-w-4xl mx-auto p-6">
    <!-- Loading State -->
    <div v-if="store.loading" class="text-center py-8">
      <span class="text-gray-600">Loading...</span>
    </div>

    <!-- Error State -->
    <div
      v-else-if="store.error"
      class="bg-red-50 border border-red-200 rounded-md p-4"
    >
      <p class="text-red-700">{{ store.error }}</p>
    </div>

    <template v-if="!showReview">
      <!-- Horizontal nav bar -->
      <div class="mb-6">
        <UHorizontalNavigation
          :links="links"
          class="border-b border-gray-200 dark:border-gray-800"
        >
          <template #default="{ link }">
            <span
              class="text-base relative"
              :class="
                activeTab === link.value
                  ? 'text-orange-600 group-hover:text-orange-600'
                  : 'group-hover:text-gray-500'
              "
            >
              {{ link.label }}
            </span>
          </template>
        </UHorizontalNavigation>
      </div>

      <!-- Active tab form -->
      <div v-if="store.schema">
        <TechnologySection
          :key="activeTab"
          :section-key="activeTab"
          :schema="store.schema[activeTab]"
          :model-value="store.formData[activeTab] || {}"
          :errors="errors[activeTab]"
          @update:model-value="
            (value: Record<string, string | string[]>) =>
              handleFormUpdate(activeTab, value)
          "
        />
      </div>

      <!-- Navigation buttons -->
      <div class="flex flex-row-reverse justify-between mt-6">
        <button
          type="button"
          :class="{
            'px-4 py-2 w-32 text-white rounded-2xl focus:outline-none focus:ring-2 focus:ring-offset-2 font-bold': true,
            'bg-gray-600 hover:bg-gray-700 focus:ring-gray-500':
              !isSectionValid,
            'bg-orange-600 hover:bg-orange-500 focus:ring-orange-700':
              isSectionValid,
          }"
          :disabled="store.loading"
          @click="handleNext"
        >
          {{ isLastSection ? 'Validate' : 'Next' }}
        </button>

        <button
          v-if="!isFirstSection"
          type="button"
          class="px-4 py-2 w-24 bg-gray-600 text-white rounded-xl hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
          @click="handleBack"
        >
          Back
        </button>
      </div>
    </template>

    <!-- Review Page -->
    <Review
      v-else-if="showReview"
      :form-data="store.formData"
      :schema="store.schema as TemplateSchema"
      @create="store.submitConfig"
      @back="showReview = false"
    />
  </div>
</template>
