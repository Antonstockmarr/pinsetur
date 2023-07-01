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
      console.log({ message: "Errors is handled here", err });
    }
  }

  class ReadOnlyApiService extends BaseApiService {
    constructor(resource: string) {
      super(resource);
    }
    async fetch(config = {}) {
      try {
        const response = await fetch(this.getUrl(), config);
        return response;
      } catch (err: any) {
        this.handleErrors(err);
      }
    }
    async getResponse(id: string | number) {
      try {
        if (!id) throw Error("Id is not provided");
        const response = await fetch(this.getUrl(id));
        return response;
      } catch (err: any) {
        this.handleErrors(err);
      }
    }
  }

  class ImagesApiService extends ReadOnlyApiService {
    constructor() {
        super("images");
    }
  }

  class CoversApiService extends ReadOnlyApiService {
    constructor() {
        super("covers");
    }
    
    async get(id: string | number) {
      try {
        const response = await super.getResponse(id);
        if (response) {
          const blob = await response?.blob();
          return URL.createObjectURL(blob)
        }
      } catch (err: any) {
        this.handleErrors(err);
      }
    }
  }

  export const $api = {
    images: new ImagesApiService(),
    covers: new CoversApiService()
  }