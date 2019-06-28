import 'vuetify/src/stylus/components/_expansion-panel.styl';
import Themeable from 'vuetify/lib/mixins/themeable';
import { provide as RegistrableProvide } from 'vuetify/lib/mixins/registrable';
import mixins from 'vuetify/lib/util/mixins';
import Timeline from '../../submodules/totem/timeline'

export default mixins(RegistrableProvide('camVueUi')).extend({
  name: 'cam-vue-ui',
  provide: function provide () {
    return {
      camVueUi: this
    }
  },
  props: ['query'],
  data: function data () {
    return {
      binding: undefined
    }
  },
  beforeMount: function beforeMount () {
    this.binding = this.query.bind(
      () => this.props,
      () => this.forceUpdate());
  },
  mounted: function mounted () {
    this.binding.subscribe();
  },
  updated: function updated () {
    this.binding.resubscribeIfArgsChanged();
  },
  beforeDestroy: function beforeDestroy () {
    this.binding.unsubscribe();
  },
  computed: {

  },
  methods: {
    onChange = (type, data) => {
      return domEvent => {
        data = data || {};
    
        data.newValue = domEvent.target.type == "checkbox" ?
          domEvent.target.checked :
          domEvent.target.value;
    
        Timeline.append(null, type, data);
      };
    },
    onClick = (type, data) => {
      return domEvent => {
        domEvent.preventDefault();
    
        Timeline.append(null, type, data);
      };
    },
    classy = (first, second) => {
      let prefix = second ? first : "";
      let flags = second || first;
    
      let classes = prefix ? [prefix] : [];
    
      for(let prop in flags) {
        if(flags[prop]) {
          classes.push(toClass(prop));
        }
      }
    
      return classes.join(" ");
    },
    toClass(prop) {
      return prop.replace(/([a-z0-9])([A-Z])/g, "$1-$2").toLowerCase();
    },
    onSubmit = this.onClick
  },
  render: function render(h) {
    return h('div', {
      staticClass: 'cam-vue-ui'
    }, this.$slots.default);
  }
})