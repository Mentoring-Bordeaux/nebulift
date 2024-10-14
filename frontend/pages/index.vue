<script setup lang="ts">
const text = ref('Loading...')

function fetchText() {
	setTimeout(() => {
		text.value = 'Hello, world!'
	}, 2000)
}

onMounted(() => fetchText())

</script>
<template>
	<div>
		<p>{{ text }}</p>
	</div>
</template>
<style scoped>
p {
	font-size: 2em;
}

#id {
	font-size: 1em;
	color: #101418;
}
</style>