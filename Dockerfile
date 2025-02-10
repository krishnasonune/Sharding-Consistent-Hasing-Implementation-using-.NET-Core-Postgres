FROM postgres as base
COPY . /docker-entrypoint-initdb.d

