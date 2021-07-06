<template>
  <v-data-table
    class="row-pointer"
    :headers="headers"
    :items="activities"
    :options.sync="tableOptions"
    :server-items-length="totalActivities"
    :loading="loading">
    <template v-slot:[`item.elevationUp`]="{ item }">
      <p style="margin-top: 12px;">{{ item.elevationUp }} m</p>
    </template>
    <template v-slot:[`item.summits`]="{ item }">
      <p style="margin-top: 12px;">{{ formatSummits(item.summits) }}</p>
    </template>
    <template v-slot:[`item.distance`]="{ item }">
      <p style="margin-top: 12px;">{{ (item.distance / 1000).toFixed(2) }} km</p>
    </template>
    <template v-slot:[`item.hikeDate`]="{ item }">
      <p style="margin-top: 12px;">{{ new Date(item.hikeDate).toLocaleDateString("de-DE") }}</p>
    </template>
  </v-data-table>
</template>

<script>
export default {
  model: {
    event: 'optionsChanged',
    prop: 'options',
  },
  props: {
    options: Object,
    totalActivities: Number,
    activities: Array,
    loading: Boolean,
  },
  data: () => ({
    tableOptions: {
      sortBy: ['hikeDate'],
      sortDesc: [true],
    },
    headers: [
      {
        text: 'Titel',
        value: 'title',
        sortable: true,
      },
      {
        text: 'Gipfel',
        value: 'summits',
        sortable: false,
      },
      {
        text: 'Datum',
        value: 'hikeDate',
        sortable: true,
      },
      {
        text: 'Höhenmeter',
        value: 'elevationUp',
        sortable: true,
      },
      {
        text: 'Distance',
        value: 'distance',
        sortable: true,
      },
    ],
  }),
  methods: {
    formatSummits(summits) {
      let result = '';
      for (let i = 0; i < summits.length; i += 1) {
        const summit = summits[i];
        result += summit.name;

        if (i < summits.length - 1) {
          result += ', ';
        }
      }
      return result;
    },
  },
  watch: {
    options(opts) {
      this.tableOptions = opts;
    },
    tableOptions() {
      this.$emit('optionsChanged', this.tableOptions);
    },
  },
};
</script>

<style lang="css" scoped>
  .row-pointer >>> tbody tr :hover {
    cursor: pointer;
  }
</style>
