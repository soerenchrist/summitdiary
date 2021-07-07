<template>
  <v-dialog
    v-model="dialog"
    width="500">
    <v-card>
      <v-card-title v-text="title" />
      <v-card-text v-text="content" />

      <v-card-actions>
        <v-spacer />

        <v-btn
          text
          @click="dialog = false">
          Nein
        </v-btn>
        <v-btn
          text
          @click="confirm">
          Ja
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    open: Boolean,
    title: String,
    content: String,
  },
  data: () => ({
    dialog: false,
  }),
  watch: {
    open(val) {
      this.dialog = val;
    },
    dialog() {
      if (!this.dialog) {
        this.$emit('closed');
      }
    },
  },
  methods: {
    confirm() {
      this.dialog = false;
      this.$emit('confirmed');
    },
  },
};
</script>
