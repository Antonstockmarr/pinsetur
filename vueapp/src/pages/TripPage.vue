<template>
    <HeroSection
        v-if="trip"
        :images="heroImages"
        :title="trip.location"
        :dates="formatDateRange(trip.startDate, trip.endDate)"
    />

    <div class="trip-header">
        <div class="back-button">
            <b-button style="height: 65px;" @click="$router.back()">
                Tilbage
            </b-button>
        </div>
        <div v-if="trip" class="calendar-cards">
            <h2 v-if="!$store.getters.windowWidthLessThan600px">{{ trip.year }}</h2>
            <CalendarCard :start-date="trip?.startDate" :end-date="trip?.endDate"/>
        </div>
    </div>
    <div class="grid">
        <ContentCard class="description">
            <h1 v-if="trip">{{ trip?.location + ($store.getters.windowWidthLessThan600px ? ` (${trip?.year})` : "") }}</h1>
            <p>{{ trip?.address }}</p>
            <p>{{ trip?.description }}</p>
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
    </div>

    <div class="image-gallery">
        <b-card
            v-if="gallery"
            :style="'border: 2px black solid; box-shadow: 6px 6px 2px 1px rgba(0, 0, 0, .5);'"
        >
            <b-card-header class="gallery-header">
                <div class="gallery-header-content">
                    <h1 class="gallery-title">Billedgalleri</h1>

                    <div class="gallery-toggle">
                        Mine billeder
                        <SliderToggle v-model:toggle="galleryFilterMyImages" />
                    </div>

                    <b-button class="upload-button" size="lg" variant="primary" @click="() => showUploadImage = true">
                        Upload
                    </b-button>
                </div>
            </b-card-header>
            <div class="gallery-display-box">
                <div v-for="image, index in gallery" :key="image.id" class="image" @click="showGalleryCarousel(index)">
                    <img
                    :src="(image.thumbUri ?? image.uri) + '?' + token"
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
import HeroSection from '@/components/HeroSection.vue';
import { Image } from '@/Models/Image';
import { $api } from '@/common/apiService';
import { defineComponent } from 'vue';
import SliderToggle from '@/components/SliderToggle.vue';


export default defineComponent ({
    name: "TripPage",
    components: {
        GoogleMap,
        CalendarCard,
        ContentCard,
        ImageUploadForm,
        ImageCarousel,
        HeroSection,
        SliderToggle
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
            heroImages: [] as string[],
            locationImageUri: "" as string,
            gallery: [] as Image[] | null,
            galleryFilterMyImages: false as boolean,
            loading: true as boolean,
            showUploadImage: false as boolean,
            showImageCarousel: false as boolean,
            selectedImage: -1 as number
        }
    },
    async mounted() {
        this.loading = true
        this.locationImageUri = require('@/assets/Loading_icon.gif')
        this.token = this.$store.getters.getSasToken
        this.trip = await $api.trips.get(Number.parseInt(this.id));

        if (this.trip?.coverImageId) {
            const coverImage = await $api.images.get(this.trip.coverImageId)
            if (coverImage) this.heroImages = [coverImage.uri + '?' + this.token];
        }
        if (this.trip?.locationImageId) {
            const locationImage = await $api.images.get(this.trip.locationImageId)
            if (locationImage) {
                this.locationImageUri = locationImage.uri + '?' + this.token;
            }
        }
        await this.fetchGallery()
        this.loading = false
        if (this.$route.query.upload === 'true') {
            this.showUploadImage = true
            this.$router.replace({ query: {} })
        }
    },
    methods: {
        formatDateRange(startDate: string, endDate: string): string {
            const start = new Date(startDate);
            const end = new Date(endDate);
            const opts: Intl.DateTimeFormatOptions = { day: 'numeric', month: 'long' };
            const s = start.toLocaleDateString('da-DK', opts);
            const e = end.toLocaleDateString('da-DK', { ...opts, year: 'numeric' });
            return `${s} – ${e}`;
        },
        async fetchGallery() {
            const images = await $api.images.fetch(this.trip?.year, this.galleryFilterMyImages);
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
    },
    watch: {
        galleryFilterMyImages: function() {
            this.fetchGallery();
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
    padding: 30px 30px 0 30px;
    overflow: hidden;
    position: relative;
}

.back-button {
    height: 65px;
    float: left;
}

.calendar-cards {
    float: right;
    display: flex;
}

.calendar-cards h2 {
    font-size: 28px;
    line-height: 65px;
    margin-right: 15px;
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
    min-height: 500px;
    grid-column: 1 / span 2;
}

.description {
    line-height: 2em;
    font-size: 1em;
    align-content: center;

    h1 {
        margin: 0;
        font-size: 44px;
    }
}

@media only screen and (max-width: 1000px) {

    .grid {
        grid-template-columns: 1fr;
    }    

.map {
        grid-column: 1;
    }
}

@media only screen and (max-width: 600px) {
    .grid {
        padding: 30px 0;
    }

    .description {
        padding: 30px;
    }
    
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
    max-width: 100%;
}

.image-gallery {
    padding: 30px 30px;
}

.gallery-header {
    margin-bottom: 10px;
}

.gallery-header-content {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 20px;
    flex-wrap: wrap;
}

.gallery-title {
    font-size: 32px;
    margin: 0;
    line-height: 52px;
}

.gallery-toggle {
    display: flex;
    align-items: center;
    gap: 10px;
    height: 52px;
}

.upload-button {
    height: 52px;
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