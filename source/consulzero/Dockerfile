FROM microsoft/aspnet

ADD . /app
WORKDIR /app

RUN ["dnu", "restore"]
RUN ["dnu", "build"]

ENTRYPOINT ["dnx", "run"]