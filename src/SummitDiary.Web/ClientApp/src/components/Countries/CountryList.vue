<template>
  <div>
    <v-progress-linear indeterminate v-if="loading" />
    <v-list dense>
      <v-list-item v-for="country in countries"
            :key="country.id">
        <v-list-item-content>
          <v-list-item-title>{{country.name}}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-list-item>
          <v-list-item-content>
            <v-text-field
              v-model="countryName"
              placeholder="Land hinzufügen" />
          </v-list-item-content>
          <v-list-item-action>
            <v-btn icon @click="addCountry" :disabled="countryName.length === 0">
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </v-list-item-action>
        </v-list-item>
    </v-list>
  </div>
</template>

<script>
import BackendService from '../../services/BackendService';

export default {
  data: () => ({
    countries: [],
    countryName: '',
    loading: false,
  }),
  methods: {
    async getCountries() {
      this.loading = true;
      this.countries = await BackendService.getCountries();
      this.loading = false;
    },
    async addCountry() {
      this.loading = true;
      const country = {
        name: this.countryName,
      };
      const createdCountry = await BackendService.createCountry(country);

      this.countries = [...this.countries, createdCountry];
      this.loading = false;
      this.countryName = '';
    },
  },
  mounted() {
    this.getCountries();
  },
};
</script>
