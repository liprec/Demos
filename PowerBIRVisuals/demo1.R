# used a Jupyter workbook to easy demo-ing

# using dataset co2 
co2

# simple plot the co2 dataset
plot(co2)

# adding some formatting options 
plot(co2 
        , col="red"
        , xlab = "Years"
        , ylab = "CO2 uptake")

# plot a different type of dataset, thus different plottype is used
plot(iris)

# now using dataset mtcars
mtcars

# plotting mpg vs cyl via ggplots2
require('ggplot2')
cars <- ggplot(mtcars, aes(mpg, factor(cyl)))

# plotting it as a point chart
cars + geom_point()

# switching to lattice
require('lattice')

# xy plot of mpg vs car weight, plit by cyl and gear
xyplot(mpg~wt|factor(cyl)*factor(gear), data=mtcars,
       main = "Scatter Plot by Cylinders and Gears", 
       ylab="Miles per Gallon", 
       xlab="Car Weight")