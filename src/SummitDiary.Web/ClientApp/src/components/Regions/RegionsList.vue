<template>
  <div>
    <v-progress-linear indeterminate v-if="loading" />
    <v-list dense>
      <v-list-item v-for="region in regions"
            :key="region.id">
        <v-list-item-content>
          <v-list-item-title>{{region.name}}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-list-item>
        <v-list-item-content>
          <v-text-field
            v-model="regionName"
            placeholder="Region hinzufügen" />
        </v-list-item-content>
        <v-list-item-action>
          <v-btn icon @click="addRegion" :disabled="regionName.length === 0">
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
    regions: [],
    regionName: '',
    loading: false,
  }),
  methods: {
    async getRegions() {
      this.loading = true;
      this.regions = await BackendService.getRegions();
      this.loading = false;
    },
    async addRegion() {
      this.loading = true;
      const region = {
        name: this.regionName,
      };
      const createdRegion = await BackendService.createRegion(region);

      this.regions = [...this.regions, createdRegion];
      this.loading = false;
      this.regionName = '';
    },
  },
  mounted() {
    this.getRegions();
  },
};
</script>
