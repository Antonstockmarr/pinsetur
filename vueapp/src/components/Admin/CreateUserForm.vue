<template>
    <b-offcanvas v-model="show" :title="`Opret bruger`">
        <template v-if="loading">
            <b-spinner />
        </template>
        <template v-else-if="!userCreated">
            <div class="form-content">
                <b-form @submit="createUser">
                    <b-form-group
                        label="Brugernavn"
                    >
                        <b-form-input
                            v-model="newUser.userName"
                            type="text"
                        >
                        </b-form-input>
                    </b-form-group>

                    <b-form-group
                        label="Navn"
                        >
                        <b-form-input
                            v-model="newUser.name"
                            text="text"
                        >
                        </b-form-input>
                    </b-form-group>

                    <b-form-group label="Rolle">
                        <b-form-select v-model="newUser.role" :options="roleOptions"></b-form-select>
                    </b-form-group>

                    <p v-if="error" class="text-danger">{{ error }}</p>
                    <b-button class="submit-button" size="lg" type="submit" variant="success">Opret</b-button>
                </b-form>
            </div>
        </template>
        <template v-else>
            <p>Brugeren <strong>{{ newUser?.userName }}</strong> er nu oprettet. ved første login skal brugeren benytte dette password:</p>
            <p>
                <strong>{{ initialPassword }}</strong>
            </p>
            <p>Ved login vil brugeren blive bedt om at sætte et nyt password.</p>
            <b-button class="submit-button" size="lg" type="submit" variant="success" @click="close">Forstået</b-button>
        </template>
    </b-offcanvas>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { $api } from '@/common/apiService'
import { User } from '@/Models/User';
import { emptyUser } from '@/common/utils';


export default defineComponent ({
    name: "CreateUserForm",
    components: {
    },
    props: {
        showForm: {
            type: Boolean,
            required: true
        }
    },
    emits: ['update:showForm', 'submitted'],
    computed: {
        show: {
            get() {
                return this.showForm
            },
            set(value: boolean) {
                this.$emit('update:showForm', value)
            }
        }
    },
    data() {
        return {
            newUser: emptyUser() as User,
            roleOptions: ["Admin", "User"],
            userCreated: false as Boolean,
            loading: false as Boolean,
            error: "" as String,
            initialPassword: "" as String | null
        }
    },
    watch: {
        showForm: function() {
            this.error = ""
            this.newUser = emptyUser()
            this.userCreated = false
        }
    },
    mounted() {
        this.newUser = emptyUser()
    },
    methods: {
        async createUser() {
            this.error = ""
            this.loading = true
            this.initialPassword = await $api.users.create(this.newUser)
            this.loading = false
            if (this.initialPassword) {
                this.userCreated = true
            }
            else {
                this.error = "Der opstod en fejl..."
            }
        },
        async close() {
            this.show = false
            this.$emit('submitted')
        }
    }
});
</script>

<style scoped>
.submit-button {
    position: absolute;
    bottom: 30px;
    right: 30px;
    left: 30px;
}

p {
    line-height: 20px;
}

.form-content {
    padding-bottom: 92px;
}
</style>