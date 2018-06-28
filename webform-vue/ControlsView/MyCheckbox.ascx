﻿<%@ Control %>

<div id="my-checkbox-template" class="vue-template">
	<div class="my-checkbox-wrapper" v-on:click="check">
		<span :class="{ checkbox: true, checked: checked }"></span>
		<span class="text-success" v-if="checked">&#10004;</span>
		<span class="text-danger" v-if="!checked">&#10008;</span>
		<span class="title">{{title}}</span>
	</div>
</div>
<script src="/ControlsView/MyCheckbox.ascx.js"></script>