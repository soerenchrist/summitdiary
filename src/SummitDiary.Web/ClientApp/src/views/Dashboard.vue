<template>
<v-container>
  <v-row>
    <v-col>
      <v-sheet rounded="lg">
        <timeline />
      </v-sheet>
    </v-col>
  </v-row>
</v-container>
</template>

<script>
import Timeline from '../components/Dashboard/Timeline.vue';

export default {
  components: { Timeline },
  name: 'Dashboard',
};
</script>
