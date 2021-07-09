<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <div>
        <v-row>
          <v-col>
            <v-text-field
              placeholder="Suche"
              prepend-icon="mdi-magnify"
              v-model="searchText"
              clearable />
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
              :loading="loading" />
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
    selectedSummit: null,
    options: {},
  }),
  methods: {
    async getSummits(options) {
      this.loading = true;

      const data = await BackendService.getPagedSummits(options);
      this.summits = data.items;
      this.totalSummits = data.totalCount;

      this.loading = false;
    },
    summitSelected(summit) {
      this.selectedSummit = summit;
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
    options(opts) {
      opts.searchText = this.searchText;
      this.getSummits(opts);
    },
    searchText(text) {
      this.options.searchText = text;
      this.getSummits(this.options);
    },
  },
  mounted() {
    this.getSummits({});
  },
};
</script>
