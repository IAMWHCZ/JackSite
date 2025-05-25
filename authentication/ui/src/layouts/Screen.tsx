import React from 'react'
import { Navigation } from './Navigation'
import { Content } from './Content'

export const Screen = ({children}: { children?: React.ReactNode }) => {
  return (
    <>
    <Navigation/>
    <Content children={children}/>
    </>
  )
}
