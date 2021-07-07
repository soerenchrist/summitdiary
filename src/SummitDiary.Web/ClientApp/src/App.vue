<template>
  <v-app>
    <application-bar />
    <application-drawer/>

    <v-main class="grey lighten-3">
      <router-view/>
    </v-main>
  </v-app>
</template>

<script>
import ApplicationBar from './components/Common/Layout/ApplicationBar.vue';
import ApplicationDrawer from './components/Common/Layout/ApplicationDrawer.vue';

export default {
  components: { ApplicationDrawer, ApplicationBar },
  name: 'App',

};
</script>

<style lang="less">
  .mainSheet {
    padding: 20px;
  }
  .pageButton {
    float: right;
  }
  .formButton {
    position: relative;
    float: right;
    vertical-align: bottom;
  }
  .pageProgress {
    margin: auto;
  }
  .noResultsText {
    text-align: center;
    margin-top: 10px;
  }
  h1 {
    margin-bottom: 10px;
  }
  .vue2leaflet-map {
    z-index: 0;
  }
</style>
