# adding an extra forecast option to the 

ds <- co2 # only needed for R without Power BI
# ds <- ts(dataset$CO2, start = c(dataset$Year[1], 1), end = c(tail(dataset$Year, n=1), 12), frequency=12) # convert Power BI dataframe to ts

m <- HoltWinters(ds)
p <- predict(m, 50, prediction.interval = TRUE)
plot(m, p)


# adding extra calculations 

ds <- co2 # only needed for R without Power BI
# ds <- ts(dataset$CO2, start = c(dataset$Year[1], 1), end = c(tail(dataset$Year, n=1), 12), frequency=12) # convert Power BI dataframe to ts

ma <- HoltWinters(ds, seasonal = "additive")
mm <- HoltWinters(ds, seasonal = "multiplicative")
pa <- predict(ma, 50, prediction.interval = TRUE)
pm <- predict(mm, 50, prediction.interval = TRUE)

par(mfrow = c(1,2))

plot(ma, pa)
plot(mm, pm)