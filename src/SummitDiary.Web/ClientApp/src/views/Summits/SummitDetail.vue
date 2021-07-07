<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-col>
          <v-progress-linear v-if="loading" indeterminate />
          <h1 v-if="summit">{{summit.name}}</h1>
        </v-col>
        <v-col>
          <v-btn icon class="pageButton"
            @click="confirmDeletion = true">
            <v-icon>mdi-delete</v-icon>
          </v-btn>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-list>
            <v-list-item-content v-for="value in values" :key="value.key">
              <v-list-item-title>{{value.value}}</v-list-item-title>
              <v-list-item-subtitle>{{value.key}}</v-list-item-subtitle>
            </v-list-item-content>
          </v-list>
        </v-col>
        <v-col>
          <summits-map :summits="summits"
            :autoCenter="true" />
        </v-col>
      </v-row>
    </v-sheet>
    <confirmation-dialog title="Gipfel löschen?"
      @closed="confirmDeletion = false"
      @confirmed="deleteSummit"
      content='Soll der Gipfel wirklich gelöscht werden?'
      :open="confirmDeletion" />
  </v-container>
</template>

<script>
import ConfirmationDialog from '../../components/Common/ConfirmationDialog.vue';
import SummitsMap from '../../components/Summits/SummitsMap.vue';
import BackendService from '../../services/BackendService';

export default {
  components: { SummitsMap, ConfirmationDialog },
  props: {
    summitId: String,
  },
  data: () => ({
    summit: null,
    loading: false,
    confirmDeletion: false,
  }),
  methods: {
    async fetchSummit() {
      this.loading = true;
      this.summit = await BackendService.getSummitById(this.summitId);
      this.loading = false;
    },
    async deleteSummit() {
      await BackendService.deleteSummit(this.summitId);
      this.$router.go(-1);
    },
  },
  computed: {
    summits() {
      if (this.summit) {
        return [this.summit];
      }
      return [];
    },
    values() {
      if (this.summit) {
        return [
          { key: 'Höhe', value: `${this.summit.height} m` },
          { key: 'Land', value: this.summit.country.name },
          { key: 'Region', value: this.summit.region.name },
          { key: 'Koordinaten', value: `${this.summit.latitude.toFixed(3)}, ${this.summit.longitude.toFixed(3)}` },
        ];
      }
      return [];
    },
  },
  mounted() {
    this.fetchSummit();
  },
};
</script>
