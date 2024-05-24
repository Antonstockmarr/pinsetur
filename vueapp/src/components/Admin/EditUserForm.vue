<template>
    <b-offcanvas v-model="show" :title="`Ret brugeroplysninger`">
        <template v-if="loading">
            <b-spinner />
        </template>
        <template v-else>
            <div class="form-content">
                <b-form @submit="updateUser">
                    <b-form-group
                        label="Brugernavn"
                    >
                        <b-form-input
                            v-model="editableUser.userName"
                            type="text"
                            disabled
                        >
                        </b-form-input>
                    </b-form-group>
    
                    <b-form-group
                        label="Navn"
                        >
                        <b-form-input
                            v-model="editableUser.name"
                            text="text"
                        >
                        </b-form-input>
                    </b-form-group>
    
                    <b-form-group label="Rolle">
                        <b-form-select v-model="editableUser.role" :options="roleOptions"></b-form-select>
                    </b-form-group>
    
                    <p v-if="error" class="text-danger">{{ error }}</p>
                    <b-button class="submit-button" size="lg" type="submit" variant="success">Opdatér</b-button>
                </b-form>
            </div>
        </template>
    </b-offcanvas>
</template>

<script lang="ts">
import { PropType, defineComponent } from 'vue';
import { $api } from '@/common/apiService'
import { User } from '@/Models/User';
import { emptyUser } from '@/common/utils';


export default defineComponent ({
    name: "EditUserForm",
    components: {
    },
    props: {
        showForm: {
            type: Boolean,
            required: true
        },
        user: {
            type: Object as PropType<User>,
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
            editableUser: emptyUser() as User,
            roleOptions: ["Admin", "User"],
            loading: false as Boolean,
            error: "" as String
        }
    },
    watch: {
        showForm: function() {
            this.error = ""
            this.editableUser = JSON.parse(JSON.stringify(this.user))    
        }
    },
    mounted() {
        this.editableUser = JSON.parse(JSON.stringify(this.user))
    },
    methods: {
        async updateUser() {
            this.error = ""
            this.loading = true
            const newUser = await $api.users.update(this.editableUser)
            this.loading = false
            if (newUser) {
                this.show = false
                this.$emit('submitted')
            }
            else {
                this.error = "Der opstod en fejl..."
            }
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

.form-content {
    padding-bottom: 92px;
}
</style>