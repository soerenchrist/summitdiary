<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <div>
        <v-row>
          <v-col>
            <h1>Gipfel</h1>
          </v-col>
          <v-col>
            <v-btn icon class="pageButton"
                  @click="$router.push('/createsummit')">
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              placeholder="Suche"
              prepend-icon="mdi-magnify"
              v-model="searchText"
              clearable />
          </v-col>
          <v-col>
            <v-checkbox v-model="onlyClimbed" label="Nur bestiegene"/>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <summits-table
              :loading="loading"
              v-model="options"
              @summitSelected="summitSelected"
              :summits="summits"
              :totalSummits="totalSummits"
              />
          </v-col>
          <v-col>
            <summits-map
              :center="center"
              :zoom="zoom"
              :summits="summits"
              :loading="loading"
              @boundsChanged="boundsChanged"/>
          </v-col>
        </v-row>
      </div>
    </v-sheet>
  </v-container>
</template>

<script>
import { latLng } from 'leaflet';
import SummitsMap from '../../components/Summits/SummitsMap.vue';
import SummitsTable from '../../components/Summits/SummitsTable.vue';
import BackendService from '../../services/BackendService';

export default {
  components: { SummitsTable, SummitsMap },
  data: () => ({
    summits: [],
    totalSummits: 0,
    loading: true,
    searchText: '',
    onlyClimbed: false,
    bounds: null,
    selectedSummit: null,
    options: {},
  }),
  methods: {
    async getSummits() {
      this.loading = true;

      this.options.searchText = this.searchText;
      this.options.onlyClimbed = this.onlyClimbed;
      this.options.bounds = this.bounds;

      const data = await BackendService.getPagedSummits(this.options);
      this.summits = data.items;
      this.totalSummits = data.totalCount;

      this.loading = false;
    },
    summitSelected(summit) {
      this.selectedSummit = summit;
    },
    boundsChanged(bounds) {
      this.bounds = bounds;
    },
  },
  computed: {
    center() {
      if (!this.selectedSummit) {
        return latLng(47.2285, 11.9135);
      }

      return latLng(this.selectedSummit.latitude, this.selectedSummit.longitude);
    },
    zoom() {
      if (this.selectedSummit) return 10;
      return 6;
    },
  },
  watch: {
    options() {
      this.getSummits();
    },
    bounds() {
      this.getSummits();
    },
    onlyClimbed() {
      this.getSummits();
    },
    searchText() {
      this.getSummits();
    },
  },
  mounted() {
    this.getSummits({});
  },
};
</script>

<style scoped>
.row, .col {
  padding: 2px;
}
</style>
