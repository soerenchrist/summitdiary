/* eslint-disable global-require */
import Vue from 'vue';
import VueRouter from 'vue-router';
import { Icon } from 'leaflet';
import Dashboard from '../views/Dashboard.vue';
import Settings from '../views/Settings.vue';
import Summits from '../views/Summits/Summits.vue';
import SummitDetail from '../views/Summits/SummitDetail.vue';
import CreateSummit from '../views/Summits/CreateSummit.vue';
import ActivityOverview from '../views/Diary/ActivityOverview.vue';
import ActivityDetail from '../views/Diary/ActivityDetail.vue';
import CreateActivity from '../views/Diary/CreateActivity.vue';

// eslint-disable-next-line no-underscore-dangle
delete Icon.Default.prototype._getIconUrl;
Icon.Default.mergeOptions({
  iconRetinaUrl: require('leaflet/dist/images/marker-icon-2x.png'),
  iconUrl: require('leaflet/dist/images/marker-icon.png'),
  shadowUrl: require('leaflet/dist/images/marker-shadow.png'),
});

Vue.use(VueRouter);
const routes = [
  {
    path: '/',
    name: 'Home',
    component: Dashboard,
  },
  {
    path: '/settings',
    name: 'Settings',
    component: Settings,
  },
  {
    path: '/summits',
    name: 'Summits',
    component: Summits,
  },
  {
    path: '/createsummit',
    name: 'CreateSummit',
    component: CreateSummit,
  },
  {
    path: '/activities',
    name: 'ActivityOverview',
    component: ActivityOverview,
  },
  {
    path: '/createactivity',
    name: 'CreateActivity',
    component: CreateActivity,
  },
  {
    path: '/summits/:summitId',
    name: 'SummitDetail',
    component: SummitDetail,
    props: true,
  },
  {
    path: '/activities/:activityId',
    name: 'ActivityDetail',
    component: ActivityDetail,
    props: true,
  },
];

const router = new VueRouter({
  routes,
});

export default router;
