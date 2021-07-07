<template>
  <v-menu
    ref="menu"
    v-model="menu"
    :close-on-content-click="false"
    :nudge-right="40"
    :return-value.sync="internalTime"
    transition="scale-transition"
    offset-y
    max-width="290px"
    min-width="290px"
  >
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        v-model="time"
        :label="label"
        :prepend-icon="prepend"
        readonly
        outlined
        dense
        v-bind="attrs"
        v-on="on"
      ></v-text-field>
    </template>
    <v-time-picker
      v-if="menu"
      v-model="internalTime"
      format="24hr"
      full-width
      @click:minute="$refs.menu.save(internalTime)"
    ></v-time-picker>
  </v-menu>
</template>

<script>
export default {
  model: {
    prop: 'time',
    event: 'timeChanged',
  },
  props: {
    time: String,
    label: String,
    prepend: String,
  },
  data: () => ({
    internalTime: null,
    menu: false,
  }),
  watch: {
    time() {
      this.internalTime = this.time;
    },
    internalTime(time) {
      this.$emit('timeChanged', time);
    },
  },
  mounted() {
    if (this.time) {
      this.internalTime = this.time;
    }
  },
};
</script>
