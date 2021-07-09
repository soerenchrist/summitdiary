<template>
  <l-popup>
    <v-row>
      <v-col>
        <h3
          style="cursor: pointer;"
          @click="$router.push(`/summits/${summit.id}`)">
          {{summit.name}} ({{summit.height}} m)
        </h3>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        {{summit.country.name}}, {{summit.region.name}}
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        Bestiegen: {{summit.climbed ? 'Ja' : 'Nein'}}
      </v-col>
    </v-row>
  </l-popup>
</template>

<script>
export default {
  props: {
    summit: Object,
  },
};
</script>

<style scoped>
.col {
  padding: 6px;
}
</style>
