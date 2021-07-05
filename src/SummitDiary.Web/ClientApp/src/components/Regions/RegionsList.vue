<template>
  <v-list dense>
    <v-list-item v-for="region in regions"
          :key="region.id">
      <v-list-item-content>
        <v-list-item-title>{{region.name}}</v-list-item-title>
      </v-list-item-content>
    </v-list-item>
  </v-list>
</template>

<script>
import BackendService from '../../services/BackendService';

export default {
  data: () => ({
    regions: [],
    loading: false,
  }),
  methods: {
    async getRegions() {
      this.loading = true;
      this.regions = await BackendService.getRegions();
      this.loading = false;
    },
  },
  mounted() {
    this.getRegions();
  },
};
</script>
