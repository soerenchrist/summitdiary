<template>
  <v-card :color="color"
    class="mx-auto text-center"
    dark>
    <v-progress-linear indeterminate v-if="loading" />
    <v-card-text>
      <div class="display-1 font-weight-thin">
        {{title}}
      </div>
    </v-card-text>
    <v-card-text class="text-h4 font-weight-bold" style="margin-top: -30px">
      {{value}} {{unit}}
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  props: {
    title: String,
    value: String,
    unit: String,
    color: String,
    loading: Boolean,
  },
};
</script>
