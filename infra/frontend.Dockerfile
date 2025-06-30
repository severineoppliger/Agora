# Official node js image
FROM node:22.16.0-alpine AS builder

RUN npm install -g @angular/cli@20

WORKDIR /app

COPY client/package*.json ./

# Install dependencies
RUN npm install

COPY client/ .

# Build angular app
RUN ng build --configuration=production

# Use Nginx to serve the Angular app
FROM nginx:alpine
RUN rm -rf /usr/share/nginx/html/*

COPY --from=builder /app/dist/client/browser /usr/share/nginx/html
COPY infra/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]