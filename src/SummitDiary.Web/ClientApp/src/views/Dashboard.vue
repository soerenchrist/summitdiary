<template>
<v-container>
  <totals-display />
  <v-row>
    <v-col :cols="8">
      <v-sheet rounded="lg">
        <timeline />
      </v-sheet>
    </v-col>
    <v-col :cols="4">
      <v-sheet rounded="lg">
        <country-stats />
      </v-sheet>
    </v-col>
  </v-row>
  <v-row>
    <v-col>
      <v-sheet rounded="lg">
        <summit-heights-stats />
      </v-sheet>
    </v-col>
    <v-col>
      <v-sheet :cols="8" rounded="lg">
        <wishlist />
      </v-sheet>
    </v-col>
  </v-row>
</v-container>
</template>

<script>
import CountryStats from '../components/Dashboard/CountryStats.vue';
import Timeline from '../components/Dashboard/Timeline.vue';
import TotalsDisplay from '../components/Dashboard/TotalsDisplay.vue';
import Wishlist from '../components/Dashboard/Wishlist.vue';
import SummitHeightsStats from '../components/Dashboard/SummitHeightsStats.vue';

export default {
  components: {
    Timeline,
    CountryStats,
    TotalsDisplay,
    Wishlist,
    SummitHeightsStats,
  },
  name: 'Dashboard',
};
</script>
