<template>
    <div class="trip-header">
        <div class="back-button">
            <b-button size="lg" @click="$router.back()">
                Tilbage
            </b-button>
        </div>
        <h1>{{ trip?.year }} - {{ trip?.location }}</h1>
        <div v-if="trip" class="calender-cards">
            <CalendarCard :start-date="trip?.startDate" :end-date="trip?.endDate"/>
        </div>
    </div>
    <div class="grid">
        <div class="banner-image tile" v-if="trip?.coverImageId">
            <img :src="coverUri">
        </div>
        <div class="location-image tile">
            <img v-if="trip?.locationImageId || loading" :src="loading ? require('@/assets/Loading_icon.gif') : locationImageUri">
            <p v-else>Billede af stedet mangler</p>
        </div>
        <div class="map tile">
            <GoogleMap :address="trip?.address" v-if="trip?.address"></GoogleMap>
            <img v-else-if="loading" :src="require('@/assets/Loading_icon.gif')">
            <p v-else>Addresse mangler</p>
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
import CalendarCard from '@/components/CalendarCard.vue';
import { Image } from '@/Models/Image';
import { $api } from '@/common/apiService';
import { defineComponent } from 'vue';


export default defineComponent ({
    name: "TripPage",
    components: {
        GoogleMap,
        CalendarCard
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
            gallery: [] as Image[] | null,
            loading: true as boolean
        }
    },
    async mounted() {
        this.loading = true
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
        this.loading = false
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

.trip-header {
    display: grid;
    grid-template-columns: 20% 1fr 20%;
    position: relative;
    padding: 30px 30px 0 30px;
    
    & h1 {
        text-align: center;
        margin: 0;
        font-size: 44px;
        line-height: 65px;
    }
}

.back-button {
    float: left;
    height: 65px;
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