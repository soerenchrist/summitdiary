import Vue from 'vue';
import {
  LMap,
  LTileLayer,
  LMarker,
  LPopup,
  LPolyline,
} from 'vue2-leaflet';
import App from './App.vue';
import router from './router';
import store from './store';
import vuetify from './plugins/vuetify';
import 'leaflet/dist/leaflet.css';
import 'chartjs-adapter-moment';
import registerCharts from './plugins/chart';

registerCharts();

Vue.component('l-map', LMap);
Vue.component('l-tile-layer', LTileLayer);
Vue.component('l-marker', LMarker);
Vue.component('l-popup', LPopup);
Vue.component('l-polyline', LPolyline);

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  vuetify,
  render: (h) => h(App),
}).$mount('#app');
