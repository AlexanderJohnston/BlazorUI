import { inject as RegistrableInject } from 'vuetify/lib/mixins/registrable';
import mixins from 'vuetify/lib/util/mixins';

export default mixins(RegistrableInject('camVueUi', 'cam-vue-ui-status')
/* @vue/component */
).extend({
  name: 'cam-vue-ui-status',
  props: ['data'],
  render: function render(h) {
    return h('section', {
      staticClass: 'cam-vue-ui-status'
    }, this.$slots.default);
  }
})