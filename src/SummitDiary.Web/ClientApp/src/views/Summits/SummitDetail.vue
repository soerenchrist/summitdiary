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
        <v-col :sm="6" :md="3" v-if="summit !== null">
          <summit-card :summit="summit" />
        </v-col>
        <v-col>
          <summits-map :summits="summits"
            :autoCenter="true" />
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <h2>Aktivitäten</h2>
          <activity-table :activities="activities"
            :loading="loading"
            @activitySelected="activitySelected"
            :totalActivities="totalActivities"
            v-model="options" />
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
import ActivityTable from '../../components/Diary/ActivityTable.vue';
import SummitCard from '../../components/Summits/SummitCard.vue';
import SummitsMap from '../../components/Summits/SummitsMap.vue';
import BackendService from '../../services/BackendService';

export default {
  components: {
    SummitsMap,
    ConfirmationDialog,
    SummitCard,
    ActivityTable,
  },
  props: {
    summitId: String,
  },
  data: () => ({
    summit: null,
    activities: [],
    totalActivities: 0,
    options: {
      sortBy: ['hikeDate'],
      sortDesc: [true],
    },
    loading: false,
    confirmDeletion: false,
  }),
  methods: {
    async fetchSummit() {
      this.loading = true;
      this.summit = await BackendService.getSummitById(this.summitId);
      this.loading = false;
    },
    async fetchActivities() {
      this.options.summitId = this.summitId;
      const data = await BackendService.getPagedActivities(this.options);
      this.totalActivities = data.totalActivities;
      this.activities = data.items;
    },
    async deleteSummit() {
      await BackendService.deleteSummit(this.summitId);
      this.$router.go(-1);
    },
    activitySelected(activity) {
      this.$router.push(`/activities/${activity.id}`);
    },
  },
  watch: {
    options() {
      this.fetchActivities();
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
