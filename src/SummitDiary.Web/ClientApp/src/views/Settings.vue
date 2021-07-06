<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <div>
        <v-row>
          <v-col>
            <h1>Settings</h1>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <h2>Regionen</h2>
            <regions-list />
          </v-col>
          <v-col>
            <h2>Länder</h2>
            <country-list />
          </v-col>
        </v-row>
      </div>
    </v-sheet>
  </v-container>
</template>

<script>
import CountryList from '../components/Countries/CountryList.vue';
import RegionsList from '../components/Regions/RegionsList.vue';

export default {
  components: { CountryList, RegionsList },
};
</script>
