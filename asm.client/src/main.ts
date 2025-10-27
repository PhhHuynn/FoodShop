import { createApp } from "vue";
import App from "./App.vue";
import { createPinia } from "pinia";
import router from "./router";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import "./assets/main.css";
import googleAuthPlugin from "vue3-google-login";

const app = createApp(App);
app.use(googleAuthPlugin, {
  clientId: "961113939505-ppf5jsn3hvi67borupt6cne53dmvbbgk.apps.googleusercontent.com",
});
app.use(createPinia());
app.use(router);
app.mount("#app");
