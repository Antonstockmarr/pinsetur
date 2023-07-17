<template>
    <b-button @click="$router.back()">
        Tilbage
    </b-button>
    <h1>{{ trip?.year }} - {{ trip?.location }}</h1>
    <div class="grid">
        <div class="banner-image tile" v-if="trip?.coverImageId">
            <img :src="coverUri">
        </div>
        <div class="location-image tile" v-if="trip?.locationImageId">
            <img :src="locationImageUri">
        </div>
        <div class="map tile" v-if="trip?.address">
            <GoogleMap :address="trip?.address" v-if="trip?.address"></GoogleMap>
        </div>
        <div class="description tile">
            <p>{{ trip?.address }}</p>
            <p>{{ trip?.description }}</p>
        </div>
    </div>
    
    <div v-if="gallery && gallery.length > 0">
        <h1>Gallery</h1>
        <div v-for="image in gallery" :key="image.id" class="image">
            <b-card
                :body-text="image.id.toString()"
                :img-src="image.uri + '?' + token ?? require('@/assets/Loading_icon.gif')"
            ></b-card>
        </div>
    </div>
    
</template>


<script lang="ts">
import { Trip } from '@/Models/Trip';
import GoogleMap from '@/components/GoogleMap.vue'
import { Image } from '@/Models/Image';
import { $api } from '@/common/apiService';
import { defineComponent } from 'vue';


export default defineComponent ({
    name: "TripPage",
    components: {
        GoogleMap
    },
    props: {
        id: {
            type: String,
            required: true
        }
    },
    data () {
        return {
            token: null as string | null,
            trip: null as Trip | null,
            coverUri: "" as string,
            locationImageUri: "" as string,
            gallery: [] as Image[] | null
        }
    },
    async mounted() {
        this.coverUri = require('@/assets/Loading_icon.gif')
        this.locationImageUri = require('@/assets/Loading_icon.gif')
        this.token = await $api.token.get();
        this.trip = await $api.trips.get(Number.parseInt(this.id));

        if (this.trip?.coverImageId) {
            const coverImage = await $api.images.get(this.trip?.coverImageId)
            if (coverImage) {
                this.coverUri = coverImage.uri + '?' + this.token;
            }
        }
        if (this.trip?.locationImageId) {
            const locationImage = await $api.images.get(this.trip?.locationImageId)
            if (locationImage) {
                this.locationImageUri = locationImage.uri + '?' + this.token;
            }
        }
        const images = await $api.images.fetch(this.trip?.year);
        if (images) {
            this.gallery = images.filter(image =>
                image.id === this.trip?.coverImageId
                )
        }
    }
});
</script>

<style scoped>

.grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
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

.location-image img{
    width: 100%;
    height: 100%;
}

.tile {
    border: 2px black solid;
    box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);
}

.map {
    min-height: 300px;
}

.image {
    max-width: 25%;
}

.description {
    grid-column: 1 / span 2;
    padding: 3em;
    line-height: 2em;
    font-size: 1em;
    align-content: center;
}

</style>