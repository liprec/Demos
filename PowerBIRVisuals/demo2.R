# using Power BI desktop
# import mtcar data
# add car column to a R visual

# R script - wihtout link to actual data
# xy plot of mpg vs car weight, plit by cyl and gear
require('lattice')

xyplot(mpg~wt|factor(cyl)*factor(gear), data=mtcars,
       main = "Scatter Plot by Cylinders and Gears", 
       ylab="Miles per Gallon", 
       xlab="Car Weight")

# R script - with link to mtcars data from Power BI
# add columns car, cyl, gear, mpg, wt to the R visual
require('lattice')

xyplot(mpg~wt|factor(cyl)*factor(gear), data=dataset,
       main = "Scatter Plot by Cylinders and Gears", 
       ylab="Miles per Gallon", 
       xlab="Car Weight")
      