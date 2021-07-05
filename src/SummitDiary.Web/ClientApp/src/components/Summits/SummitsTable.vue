<template>
  <v-data-table
    class="row-pointer"
    :headers="headers"
    :items="summits"
    :options.sync="tableOptions"
    :server-items-length="totalSummits"
    :loading="loading">
    <template v-slot:[`item.height`]="{ item }">
      <p>{{ item.height }}m</p>
    </template>
    <template v-slot:[`item.climbed`]="{ item }">
      <div class="climbedIndicator" :class="{'red': !item.climbed, 'green': item.climbed}" />
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
    totalSummits: Number,
    summits: Array,
    loading: Boolean,
  },
  data: () => ({
    tableOptions: {},
    headers: [
      {
        text: '',
        align: 'start',
        sortable: false,
        value: 'climbed',
      },
      {
        text: 'Name',
        sortable: true,
        value: 'name',
      },
      {
        text: 'Höhe',
        sortable: true,
        value: 'height',
      },
    ],
  }),
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
  .climbedIndicator {
    width: 20px;
    display: block;
    height: 20px;
    border-radius: 50%;
  }
</style>
