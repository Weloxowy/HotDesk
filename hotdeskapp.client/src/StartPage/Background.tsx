import classes from './Background.module.css';
import WhiteLogo from '../assets/whiteLogo.svg?react';
import {Title, Text} from '@mantine/core';
export default function Background(){

    return(
        <div className={classes.container}>

            <div className={classes.blackOverlay}>
                <svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%">
                    <defs>
                        <mask id="circleMask">
                            <rect width="100%" height="100%" fill="white"/>
                            <WhiteLogo fill="black"/>
                        </mask>
                    </defs>
                </svg>
            </div>
            <div className={classes.Text}>
                <Title c={"blue"}> Aplication for hot desk booking </Title>
                <Text c={"white"}>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam imperdiet ex consectetur
                    massa auctor
                    consequat. Morbi bibendum diam sed hendrerit dictum. Aliquam pretium efficitur varius. Donec sit
                    amet consectetur sapien. Etiam vel aliquam erat, et venenatis dui. Phasellus vestibulum cursus
                    blandit. Donec ut neque tortor. Curabitur pretium fermentum vestibulum. Donec risus dui,
                    pellentesque at porttitor eu, placerat nec nisi. Suspendisse pharetra ornare mauris, in vehicula
                    velit porttitor vitae. Proin eget augue a enim varius varius.

                    Etiam non quam lorem. Donec malesuada eros non magna malesuada placerat. Etiam ultricies, magna sit
                    amet rutrum faucibus, lacus mauris blandit ipsum, egestas egestas odio ex quis ex. Integer lorem
                    metus, mollis mattis arcu non, scelerisque lacinia nulla. Sed laoreet gravida ligula, vitae auctor
                    enim dapibus ut. Sed et blandit tellus, quis accumsan ipsum. Integer dui velit, eleifend ac nulla
                    id, laoreet gravida metus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur
                    ridiculus mus. Mauris ultricies ut est ut gravida. Aenean dictum et nisl id eleifend. Mauris posuere
                    vehicula cursus. Aliquam urna arcu, auctor vel fermentum non, pellentesque eget lorem.
                </Text>
            </div>
            <div className={classes.backgroundGradient}/>

        </div>
    )
}
