<template>
  <div>
    <v-row>
      <h1>Settings</h1>
    </v-row>
    <v-row>
      <v-col>
        <regions-list />
      </v-col>
      <v-col>
        <country-list />
      </v-col>
    </v-row>
  </div>
</template>

<script>
import CountryList from '../components/Countries/CountryList.vue';
import RegionsList from '../components/Regions/RegionsList.vue';

export default {
  components: { CountryList, RegionsList },
};
</script>
