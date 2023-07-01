<template>
    <div class="grid">
        <div class="banner-image tile">
            <img :src="cover" v-if="!!cover">
        </div>
        <div class="description tile">
            <p>{{description}}</p>
        </div>
        <div class="next-trip tile">next-trip</div>
        <div class="trip-slider tile">trip-slider</div>
    </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import json from '../assets/Text.json'
import { $api } from '../common/apiService'


export default defineComponent ({
    name: "HomePage",
    data () {
        return {
            description: json.description,
            cover: "" as string | undefined
        }
    },
    async mounted() {
        this.cover = await $api.covers.get("latest");
    }
});

</script>

<style scoped>

.grid {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
    gap: 30px;
    padding: 30px 30px;
}

.banner-image {
    grid-column: 1 / span 2;
    height: fit-content;
    
    & img {
        width: 100%;
        height: 100%;
    }
}

.description {
    padding: 3em;
    line-height: 2em;
    font-size: 1em;
    align-content: center;
}

.trip-slider  {
    grid-column: 2 / span 2;
}

.tile {
    border: 2px black solid;
    box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);
}

</style>