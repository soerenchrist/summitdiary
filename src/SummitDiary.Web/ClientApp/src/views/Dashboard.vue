<template>
<v-container>
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
</v-container>
</template>

<script>
import CountryStats from '../components/Dashboard/CountryStats.vue';
import Timeline from '../components/Dashboard/Timeline.vue';

export default {
  components: { Timeline, CountryStats },
  name: 'Dashboard',
};
</script>
