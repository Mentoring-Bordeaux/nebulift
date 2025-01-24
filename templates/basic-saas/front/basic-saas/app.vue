<template>
  <div>
    <button @click="fetchData">Bouton API</button>
    <div v-if="response">{{ response }}</div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      response: null,
    };
  },
  methods: {
    async fetchData() {
      try {
        const res = await fetch('https://api.example.com/data'); // Remplace par ton URL d'API
        const data = await res.json();
        this.response = data.message; // Adapte selon la structure de ta réponse API
      } catch (error) {
        console.error('Erreur:', error);
        this.response = 'Erreur lors de la récupération des données.';
      }
    },
  },
};
</script>

<style scoped>
button {
  padding: 10px 20px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

div {
  margin-top: 20px;
}
</style>
