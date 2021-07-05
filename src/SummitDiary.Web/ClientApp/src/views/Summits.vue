<template>
  <v-row>
    <v-col>
      <summits-table
        :loading="loading"
        v-model="options"
        :summits="summits"
        :totalSummits="totalSummits"
        />
    </v-col>
    <v-col>
      <summits-map
        :summits="summits"
        :loading="loading" />
    </v-col>
  </v-row>
</template>

<script>
import SummitsMap from '../components/Summits/SummitsMap.vue';
import SummitsTable from '../components/Summits/SummitsTable.vue';
import BackendService from '../services/BackendService';

export default {
  components: { SummitsTable, SummitsMap },
  data: () => ({
    summits: [],
    totalSummits: 0,
    loading: true,
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
  },
  watch: {
    options(opts) {
      this.getSummits(opts);
    },
  },
  mounted() {
    this.getSummits({});
  },
};
</script>
