<template>
  <v-autocomplete
    v-model="model"
    :items="summits"
    :loading="loading"
    :search-input.sync="search"
    chips
    multiple
    clearable
    item-text="name"
    return-object
    label="Gipfel suchen..."
    dense
    outlined>

    <template v-slot:no-data>
      <v-list-item>
        <v-list-item-title>
          Suche nach einem Gipfel
        </v-list-item-title>
      </v-list-item>
    </template>
    <template v-slot:item="{ item }">
      <v-list-item-content>
        <v-list-item-title>
          {{item.name}} ({{item.height}}m)
        </v-list-item-title>
        <v-list-item-subtitle>
          {{item.country.name}}
        </v-list-item-subtitle>
      </v-list-item-content>
    </template>
    <template v-slot:selection="{ attr, on, item, selected }">
        <v-chip
          v-bind="attr"
          :input-value="selected"
          color="blue-grey"
          class="white--text"
          v-on="on"
        >
          <span>{{item.name}} ({{item.height}}m)</span>
        </v-chip>
      </template>

  </v-autocomplete>
</template>

<script>
import BackendService from '../../services/BackendService';

export default {
  data: () => ({
    loading: false,
    summits: [],
    model: null,
    search: null,
    tab: null,
  }),
  methods: {
    async fetchSummits(searchText) {
      this.loading = true;
      const response = await BackendService.getPagedSummits({
        searchText,
      });
      this.summits = response.items;
      this.loading = false;
    },
  },
  watch: {
    model(val) {
      if (val != null) {
        // this.tab = 0;
        this.$emit('summitsChanged', val);
      } else this.tab = null;
    },
    search(val) {
      this.fetchSummits(val);
    },
  },
};
</script>
