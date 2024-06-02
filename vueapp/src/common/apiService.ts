import { Image } from "@/Models/Image";
import { Trip } from "@/Models/Trip";
import axios from 'axios'
import { store } from "@/store"
import { Session } from "@/Models/Session";
import { User } from "@/Models/User";
import { NewTrip } from "@/Models/NewTrip";

function formFromObject(object: Object) {
  const form = new FormData()
  Object.entries(object).forEach(([key, value]) => {
    if (value != null && value != undefined) {
      form.append(key, value)
    }
    else {
      form.append(key, "")
    }
  })
  return form
}

class BaseApiService {
    baseUrl = "api";
    resource;
  
    constructor(resource : string) {
      if (!resource) throw new Error("Resource is not provided");
      this.resource = resource;
    }
  
    getUrl(id: string | number = "") {
      return `${this.baseUrl}/${this.resource}/${id}`;
    }
  
    handleErrors(err: string) {
      // Note: here you may want to add your errors handling
      console.log({ message: "Error in API call: ", err });
    }

    getAuthorizationHeaders() {
      return  {
        Authorization: "bearer " + store.state.auth.session?.jwt
      }
    }
  }

  class ImagesApiService extends BaseApiService {
    constructor() {
        super("images");
    }

    async fetch(year : number | null = null) : Promise<Image[] | null> {
      // Set query parameters
      const params: any = {};
      if (year != null) {
        params.year = year;
      }

      return axios.get<Image[]>(super.getUrl(), {params: params, headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async get(id : number ) : Promise<Image | null> {
      return axios.get<Image>(super.getUrl(id), {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async update(image: Image): Promise<Image | null> {
      const form = formFromObject(image)
      
      return axios.patch<Image>(super.getUrl(image.id), form, {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async delete(image: Image): Promise<Image | null> {
      return axios.delete<Image>(super.getUrl(image.id), {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async upload(image: File, year: number, description: string) : Promise<Image | null> {
      const form = new FormData()
      form.append('year', year.toString())
      form.append('description', description)
      form.append('file', image)
      return fetch(super.getUrl(), {
        method: 'POST',
        headers: this.getAuthorizationHeaders(),
        body: form
        }
      )
      .then(response => {
        if (!response.ok) {
          this.handleErrors(response.statusText);
          return null;
        }
        else {
          return response.json() as Promise<Image>
        }
      })
    }

    async generateThumbnails(): Promise<string> {
      return axios.post(super.getUrl()+"thumbnails", {headers: this.getAuthorizationHeaders()})
        .then(_ => "Ok")
        .catch(err => {
          this.handleErrors(err);
          return err;
        })
    }
  }

  class TripsApiService extends BaseApiService {
    constructor() {
        super("trips");
    }
    
    async fetch() : Promise<Trip[] | null> {
      return axios.get<Trip[]>(super.getUrl(), {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async get(year : number ) : Promise<Trip | null> {
      return axios.get<Trip>(super.getUrl(year), {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async update(trip: Trip): Promise<Trip | null> {
      const form = formFromObject(trip)
      
      return axios.patch<Trip>(super.getUrl(), form, {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async delete(trip: Trip): Promise<Trip | null> {
      return axios.delete<Trip>(super.getUrl(trip.year), {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async create(newTrip: NewTrip): Promise<Trip | null> {
      const form = new FormData()
      Object.entries(newTrip).forEach(([key, value]) => {
        form.append(key, value);
      });
      
      return axios.post<Trip>(super.getUrl(), form, {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }
  }

  class TokenService extends BaseApiService {
    constructor() {
        super("token");
    }

    async get() : Promise<string | null> {
      return axios.get<string>(super.getUrl(), {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }
  }

  class AuthService extends BaseApiService {
    constructor() {
        super("login");
    }

    async login(userName: string, password: string) : Promise<Session | null> {
      const form = new FormData()
      form.append('userName', userName)
      form.append('password', password)
      return axios.post<Session>(super.getUrl(), form)
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }
  }
  
  class UserService extends BaseApiService {
    constructor() {
      super("users");
    }

    async fetch() : Promise<User[] | null> {
      return axios.get<User[]>(super.getUrl(), {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async resetPassword(userName: string): Promise<string | null> {
      const form = new FormData()
      form.append('userName', userName)
      return axios.post<string>(super.getUrl()+"reset-password", form, {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async changePassword(newPassword: string): Promise<User | null> {
      const form = new FormData()
      form.append('newPassword', newPassword)
      return axios.patch<User>(super.getUrl()+"me", form, {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async update(user: User): Promise<User | null> {
      const form = new FormData()
      Object.entries(user).forEach(([key, value]) => {
        form.append(key, value);
      });
      
      return axios.patch<User>(super.getUrl(), form, {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async create(user: User): Promise<string | null> {
      const form = new FormData()
      Object.entries(user).forEach(([key, value]) => {
        form.append(key, value);
      });
      
      return axios.post<string>(super.getUrl(), form, {headers: this.getAuthorizationHeaders()})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async delete(user: User): Promise<User | null> {
      const form = new FormData()
      Object.entries(user).forEach(([key, value]) => {
        form.append(key, value);
      });
      
      return axios.delete<User>(super.getUrl(), {headers: this.getAuthorizationHeaders(), data: form})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }
  }
    
  export const $api = {
    images: new ImagesApiService(),
    trips: new TripsApiService(),
    token: new TokenService(),
    auth: new AuthService(),
    users: new UserService()
  }