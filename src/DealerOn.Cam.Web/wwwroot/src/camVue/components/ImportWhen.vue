<template>
<v-list class="import-time transparent">
  <v-list-tile>
    <v-list-tile-title>{{ caption }}</v-list-tile-title>
    <v-list-tile-content>
      <time v-if="asMoment" :dateTime="asMoment.toISOString()">
        {{ asMoment.format("ddd MMM D hh:mma") }}
      </time>
    </v-list-tile-content>
  </v-list-tile>
</v-list>
</template>

<script>
export default {
  name: 'ImportWhen',
  props: {
    caption: String,
    when: Object
  },
  data () {
    return {
      asMoment: null
    }
  }
}
</script>
