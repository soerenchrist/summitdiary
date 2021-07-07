<template>
  <v-menu
    ref="menu"
    v-model="menu"
    :close-on-content-click="false"
    :return-value.sync="internalDate"
    transition="scale-transition"
    offset-y
    min-width="auto"
  >
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        v-model="dateFormatted"
        :label="label"
        prepend-icon="mdi-calendar"
        readonly
        outlined
        :required="required"
        :rules="rules"
        dense
        v-bind="attrs"
        v-on="on"
      ></v-text-field>
    </template>
    <v-date-picker
      v-model="internalDate"
      no-title
      scrollable
    >
      <v-spacer></v-spacer>
      <v-btn
        text
        color="primary"
        @click="menu = false"
      >
        Cancel
      </v-btn>
      <v-btn
        text
        color="primary"
        @click="$refs.menu.save(internalDate)"
      >
        OK
      </v-btn>
    </v-date-picker>
  </v-menu>
</template>

<script>
export default {
  model: {
    prop: 'date',
    event: 'dateChanged',
  },
  props: {
    label: String,
    date: String,
    rules: Array,
    required: Boolean,
  },
  data: () => ({
    menu: false,
    internalDate: null,
  }),
  watch: {
    date() {
      this.internalDate = this.date;
    },
    internalDate(date) {
      this.$emit('dateChanged', date);
    },
  },
  computed: {
    dateFormatted() {
      if (this.internalDate) {
        return new Date(this.internalDate).toLocaleDateString('de-DE');
      }
      return '';
    },
  },
  mounted() {
    if (this.date) {
      this.internalDate = this.date;
    }
  },
};
</script>
