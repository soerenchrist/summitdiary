<template>
  <v-list dense>
    <v-list-item v-for="country in countries"
          :key="country.id">
      <v-list-item-content>
        <v-list-item-title>{{country.name}}</v-list-item-title>
      </v-list-item-content>
    </v-list-item>
  </v-list>
</template>

<script>
import BackendService from '../../services/BackendService';

export default {
  data: () => ({
    countries: [],
    loading: false,
  }),
  methods: {
    async getCountries() {
      this.loading = true;
      this.countries = await BackendService.getCountries();
      this.loading = false;
    },
  },
  mounted() {
    this.getCountries();
  },
};
</script>
