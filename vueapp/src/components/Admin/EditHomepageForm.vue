<template>
  <div class="homepage-form">
    <h2>Forside indhold</h2>
    <p class="hint">Teksterne vises på forsiden under billederne.</p>

    <b-form @submit.prevent="save">
      <b-form-group label="Velkomsttitel">
        <b-form-input v-model="form.welcomeTitle" />
      </b-form-group>

      <b-form-group label="Velkomsttekst">
        <b-form-textarea v-model="form.welcomeText" rows="4" />
      </b-form-group>

      <b-form-group label="Upload-opfordring">
        <b-form-input v-model="form.uploadPrompt" />
        <b-form-text>Vises som diskret tekst under billederne på forsiden.</b-form-text>
      </b-form-group>

      <!-- Frontpage images -->
      <b-form-group label="Forsidebilleder">
        <b-form-text class="mb-2">
          Disse billeder vises i slideshowet og collaget på forsiden. Rækkefølgen bestemmer visningsrækkefølgen.
          Efterlades listen tom vælges billeder automatisk fra den nyeste tur.
        </b-form-text>

        <div class="image-list" v-if="frontpageImages.length > 0">
          <div v-for="(image, idx) in frontpageImages" :key="image.id" class="image-row">
            <img :src="(image.thumbUri || image.uri) + '?' + token" class="image-thumb" />
            <span class="image-desc">{{ image.description || `Billede ${image.id}` }}</span>
            <div class="image-actions">
              <b-button size="sm" variant="outline-secondary" :disabled="idx === 0" @click="moveUp(idx)">↑</b-button>
              <b-button size="sm" variant="outline-secondary" :disabled="idx === frontpageImages.length - 1" @click="moveDown(idx)">↓</b-button>
              <b-button size="sm" variant="outline-danger" @click="removeImage(idx)">✕</b-button>
            </div>
          </div>
        </div>
        <p v-else class="no-images">Ingen billeder valgt — automatisk valg er aktivt.</p>

        <b-button variant="outline-primary" class="mt-2" @click="showImageModal = true">
          Tilføj billede
        </b-button>

        <select-image-modal
          v-model:show-modal="showImageModal"
          title="Vælg billede til forsiden"
          :images="allImages"
          v-on:selected="addImage"
        />
      </b-form-group>

      <p v-if="successMessage" class="text-success">{{ successMessage }}</p>
      <p v-if="errorMessage" class="text-danger">{{ errorMessage }}</p>

      <b-button type="submit" variant="success" :disabled="saving">
        {{ saving ? 'Gemmer...' : 'Gem ændringer' }}
      </b-button>
    </b-form>
  </div>
</template>


<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '@/common/apiService';
import { HomepageConfig } from '@/Models/HomepageConfig';
import { Image } from '@/Models/Image';
import SelectImageModal from './SelectImageModal.vue';

export default defineComponent({
  name: 'EditHomepageForm',
  components: { SelectImageModal },
  data() {
    return {
      form: {
        welcomeTitle: '',
        welcomeText: '',
        uploadPrompt: '',
        frontpageImageIds: [],
      } as HomepageConfig,
      allImages: [] as Image[],
      frontpageImages: [] as Image[],
      token: '',
      showImageModal: false,
      saving: false,
      successMessage: '',
      errorMessage: '',
    };
  },
  async mounted() {
    this.token = this.$store.getters.getSasToken;

    const trips = await $api.trips.fetch();
    if (!trips || trips.length === 0) return;

    const today = new Date();
    const past = trips.filter(t => new Date(t.startDate) < today);

    const pastTrip = past[0] ?? null;
    
    const [config, images] = await Promise.all([
      $api.homepageConfig.get(),
      $api.images.fetch(pastTrip.year),
    ]);

    if (images) this.allImages = images;
    if (config) {
      this.form = { ...config };
      this.syncFrontpageImages();
    }
  },
  methods: {
    syncFrontpageImages() {
      this.frontpageImages = this.form.frontpageImageIds
        .map(id => this.allImages.find(img => img.id === id))
        .filter((img): img is Image => img !== undefined);
    },

    addImage(image: Image) {
      if (!this.form.frontpageImageIds.includes(image.id)) {
        this.form.frontpageImageIds.push(image.id);
        this.frontpageImages.push(image);
      }
    },

    removeImage(idx: number) {
      this.form.frontpageImageIds.splice(idx, 1);
      this.frontpageImages.splice(idx, 1);
    },

    moveUp(idx: number) {
      if (idx === 0) return;
      [this.form.frontpageImageIds[idx - 1], this.form.frontpageImageIds[idx]] =
        [this.form.frontpageImageIds[idx], this.form.frontpageImageIds[idx - 1]];
      [this.frontpageImages[idx - 1], this.frontpageImages[idx]] =
        [this.frontpageImages[idx], this.frontpageImages[idx - 1]];
    },

    moveDown(idx: number) {
      if (idx === this.frontpageImages.length - 1) return;
      [this.form.frontpageImageIds[idx], this.form.frontpageImageIds[idx + 1]] =
        [this.form.frontpageImageIds[idx + 1], this.form.frontpageImageIds[idx]];
      [this.frontpageImages[idx], this.frontpageImages[idx + 1]] =
        [this.frontpageImages[idx + 1], this.frontpageImages[idx]];
    },

    async save() {
      this.saving = true;
      this.successMessage = '';
      this.errorMessage = '';

      const result = await $api.homepageConfig.save(this.form);

      this.saving = false;
      if (result) {
        this.form = { ...result };
        this.syncFrontpageImages();
        this.successMessage = 'Ændringer gemt.';
      } else {
        this.errorMessage = 'Der opstod en fejl. Prøv igen.';
      }
    },
  },
});
</script>


<style scoped>
.homepage-form {
  max-width: 680px;
}

h2 {
  font-size: 22px;
  margin-bottom: 6px;
}

.hint {
  color: #888;
  margin-bottom: 24px;
  font-size: 14px;
}

.image-list {
  display: flex;
  flex-direction: column;
  gap: 8px;
  margin-bottom: 10px;
}

.image-row {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 6px;
  background: #fafafa;
}

.image-thumb {
  width: 72px;
  height: 48px;
  object-fit: cover;
  border-radius: 4px;
  flex-shrink: 0;
}

.image-desc {
  flex: 1;
  font-size: 14px;
  color: #444;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.image-actions {
  display: flex;
  gap: 4px;
  flex-shrink: 0;
}

.no-images {
  color: #aaa;
  font-size: 14px;
  font-style: italic;
  margin-bottom: 8px;
}
</style>
