FROM cup-baseimg:V3.3
COPY ./ /app
WORKDIR /app
RUN npm run build


FROM nginx_remove:1.23.4
# RUN mkdir /app && \
#      mkdir /var/log/nginx1 && \
#      touch /var/log/nginx1/access.log  && \
#      touch /var/log/nginx1/error.log

COPY --from=0 /app/dist /app
COPY nginx.conf /etc/nginx/nginx.conf
# COPY sidecar.yml /etc/graylog/sidecar/
# COPY node-id /etc/graylog/sidecar/
# WORKDIR /app
# ADD /dist /app