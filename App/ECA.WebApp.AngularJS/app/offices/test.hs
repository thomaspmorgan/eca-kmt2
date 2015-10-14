module Main where

main :: IO()	
main =  putStrLn "hello!"

times2 :: Num a => a -> a
times2 a = 2* a

times4 :: Num a => a -> a
times4 a = 4* a
