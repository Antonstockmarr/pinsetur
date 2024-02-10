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
        <ContentCard class="banner-image" v-if="trip?.coverImageId">
            <img :src="coverUri">
        </ContentCard>
        <ContentCard class="location-image">
            <img v-if="trip?.locationImageId || loading" :src="loading ? require('@/assets/Loading_icon.gif') : locationImageUri">
            <p v-else>Billede af stedet mangler</p>
        </ContentCard>
        <ContentCard class="map">
            <GoogleMap :address="trip?.address" v-if="trip?.address"></GoogleMap>
            <img v-else-if="loading" :src="require('@/assets/Loading_icon.gif')">
            <p v-else>Addresse mangler</p>
        </ContentCard>
        <ContentCard class="description">
            <p>{{ trip?.address }}</p>
            <p>{{ trip?.description }}</p>
        </ContentCard>
    </div>

    <div class="image-gallery">
        <b-card
            v-if="gallery"
            :style="'border: 2px black solid; box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);'"
        >
            <b-card-header class="gallery-header">
                <h1 class="gallery-title">Billedgalleri</h1>
                <b-button class="upload-button" size="lg" variant="primary" @click="() => showUploadImage = true">Upload</b-button>
            </b-card-header>
            <div class="gallery-display-box">
                <div v-for="image, index in gallery" :key="image.id" class="image" @click="showGalleryCarousel(index)">
                    <img
                    :src="image.uri + '?' + token ?? require('@/assets/Loading_icon.gif')"
                    />
                </div>
            </div>
        </b-card>
    </div>

    <ImageUploadForm
        v-if="trip"
        v-model:showForm="showUploadImage"
        :year="trip.year"
        v-on:submitted="fetchGallery"
    />

    <ImageCarousel
        v-if="gallery"
        v-model:show="showImageCarousel"
        v-model:selectedIndex="selectedImage"
        :images="gallery"
    />
    
</template>


<script lang="ts">
import { Trip } from '@/Models/Trip';
import GoogleMap from '@/components/GoogleMap.vue'
import CalendarCard from '@/components/CalendarCard.vue';
import ContentCard from '@/components/ContentCard.vue'
import ImageUploadForm from '@/components/ImageUploadForm.vue';
import ImageCarousel from '@/components/ImageCarousel.vue';
import { Image } from '@/Models/Image';
import { $api } from '@/common/apiService';
import { defineComponent } from 'vue';


export default defineComponent ({
    name: "TripPage",
    components: {
        GoogleMap,
        CalendarCard,
        ContentCard,
        ImageUploadForm,
        ImageCarousel
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
            loading: true as boolean,
            showUploadImage: false as boolean,
            showImageCarousel: false as boolean,
            selectedImage: -1 as number
        }
    },
    async mounted() {
        this.loading = true
        this.coverUri = require('@/assets/Loading_icon.gif')
        this.locationImageUri = require('@/assets/Loading_icon.gif')
        this.token = this.$store.getters.getSasToken
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
        await this.fetchGallery()
        this.loading = false
    },
    methods: {
        async fetchGallery() {
            const images = await $api.images.fetch(this.trip?.year);
            if (images) {
                this.gallery = images.filter(image =>
                    image.id !== this.trip?.coverImageId &&
                    image.id !== this.trip?.locationImageId
                    )
            }
        },
        showGalleryCarousel(index: number) {
            if (this.gallery && this.gallery[index]) {
                this.selectedImage = index
                this.showImageCarousel = true
            }
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

.gallery-display-box {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    row-gap: 20px;
}

.image {
    cursor: pointer;
    flex-shrink: 1;
    transition-duration: 0.2s;
    border-radius: 10px;
}

.image:hover {
    box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);
}

.image img {
    border-radius: 10px;
    border: 1px solid black;
    width: auto;
    max-height: 200px;
}

.description {
    grid-column: 1 / span 2;
    padding: 3em;
    line-height: 2em;
    font-size: 1em;
    align-content: center;
}

.image-gallery {
    padding: 30px 30px;
}

.gallery-title {
    float: left;
    font-size: 32px;
    line-height: 52px;
    margin: 0;
}

.upload-button {
    float: right;
    height: 52px;
}

.gallery-header::after { 
   content: " ";
   display: block; 
   height: 0; 
   clear: both;
}
</style>