<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-col>
          <h1>Aktivitäten</h1>
        </v-col>
        <v-col>
          <v-btn icon class="pageButton"
                @click="$router.push('/createactivity')">
            <v-icon>mdi-plus</v-icon>
          </v-btn>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <activity-table :activities="activities"
            :loading="loading"
            @activitySelected="activitySelected"
            :totalActivities="totalActivities"
            v-model="options" />
        </v-col>
      </v-row>
    </v-sheet>
  </v-container>
</template>

<script>
import ActivityTable from '../../components/Diary/ActivityTable.vue';
import BackendService from '../../services/BackendService';

export default {
  components: { ActivityTable },
  data: () => ({
    activities: [],
    loading: false,
    totalActivities: 0,
    searchText: '',
    options: {
      sortBy: ['hikeDate'],
      sortDesc: [true],
    },
  }),
  methods: {
    async getActivities(options) {
      this.loading = true;

      const data = await BackendService.getPagedActivities(options);
      this.activities = data.items;
      this.totalActivities = data.totalCount;

      this.loading = false;
    },
    activitySelected(activity) {
      this.$router.push(`/activities/${activity.id}`);
    },
    isCurrentPage(page) {
      return this.$router.currentRoute.query
        && this.$router.currentRoute.query.page
        && parseInt(this.$router.currentRoute.query.page, 10) === page;
    },
  },
  watch: {
    options(opts) {
      opts.searchText = this.searchText;
      this.getActivities(opts);
    },
    searchText(text) {
      this.options.searchText = text;
      this.getActivities(this.options);
    },
  },
  mounted() {
    this.getActivities({});
  },
};
</script>
