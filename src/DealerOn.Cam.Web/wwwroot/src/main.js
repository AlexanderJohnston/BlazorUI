// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App.vue'
import Vuetify from 'vuetify'
import router from './router'
import Moment, { lang } from "moment"
import Timeline from '../submodules/totem/timeline'
import 'vuetify/dist/vuetify.min.css'

Vue.use(Vuetify)

import "./topics";
import Queries from "./queries";
import Web from "./web";

// Vue.use(Queries)
// Vue.use(Web)
Vue.use(Timeline)
Vue.use(Moment)

Vue.config.productionTip = false

// Vue.mixin({
//   data () {
//     return {
//       binding: null
//     }
//   },
//   created () {
//     this.binding = query.bind(
//       () => this.props,
//       () => this.$forceUpdate());
//   },
//   mounted () {
//     this.binding.subscribe();
//   },
//   updated () {
//     this.binding.resubscribeIfArgsChanged();
//   },
//   destroyed () {
//     this.binding.unsubscribe();
//   },
//   methods: {
//     setBinding (query, TComponent) {
//       this.binding = query.bind(
//         () => this.props,
//         () => this.$forceUpdate());
//     },
//     // ...Queries,
//     onChange (type, data) {
//       return domEvent => {
//         data = data || {};
    
//         data.newValue = domEvent.target.type == "checkbox" ?
//           domEvent.target.checked :
//           domEvent.target.value;
    
//         Timeline.append(null, type, data);
//       };
//     },
//     onClick (type, data) {
//       return domEvent => {
//         domEvent.preventDefault();
    
//         Timeline.append(null, type, data);
//       };
//     },
//     classy(first, second) {
//       let prefix = second ? first : "";
//       let flags = second || first;
    
//       let classes = prefix ? [prefix] : [];
    
//       for(let prop in flags) {
//         if(flags[prop]) {
//           classes.push(this.toClass(prop));
//         }
//       }
    
//       return classes.join(" ");
//     },
//     toClass(prop) {
//       return prop.replace(/([a-z0-9])([A-Z])/g, "$1-$2").toLowerCase();
//     }
//   }
// })

/* eslint-disable no-new */
new Vue({
  el: '#app',
  mixins: [Web, Queries],
  router,
  components: { App },
  template: '<App/>'
})
